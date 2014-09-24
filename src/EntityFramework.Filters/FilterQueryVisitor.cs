namespace EntityFramework.Filters
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core.Common.CommandTrees;
    using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Core.Objects;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using System.Reflection;

    public class FilterQueryVisitor : DefaultExpressionVisitor
    {
        private readonly DbContext _contextForInterception;
        private readonly ObjectContext _objectContext;

        public FilterQueryVisitor(DbContext contextForInterception)
        {
            _contextForInterception = contextForInterception;
            _objectContext = ((IObjectContextAdapter)contextForInterception).ObjectContext;
        }

        public override DbExpression Visit(DbScanExpression expression)
        {
            // a bit harder to get the metadata in CSpace
            var item = expression.Target.ElementType.MetadataProperties.First(p => p.Name == "Configuration");

            // using reflection to get the Annotations property as EntityTtypeConfiguration is an internal class in EF
            Dictionary<string, object> annotations = new Dictionary<string, object>();
            var value = item.Value;
            var propertyInfo = value.GetType().GetProperty("Annotations");
            if (propertyInfo != null)
            {
                annotations = (Dictionary<string, object>) propertyInfo.GetValue(value, null);
            }

            if (!annotations.Any())
            {
                return base.Visit(expression);
            }

            DbExpression current = expression;
            foreach (var globalFilter in annotations.Where(a => a.Key.StartsWith("globalFilter")))
            {
                var convention = (FilterDefinition)globalFilter.Value;

                Filter filterConfig;

                string filterName = globalFilter.Key.Split(new[] { "globalFilter_" }, StringSplitOptions.None)[1];

                if (!FilterExtensions.FilterConfigurations.TryGetValue(new Tuple<string, object>(filterName, _contextForInterception.GetInternalContext()), out filterConfig))
                    continue;

                if (!filterConfig.IsEnabled)
                    continue;

                var linqExpression = convention.Predicate(_contextForInterception, filterConfig.ParameterValues);

                var funcletizerType = typeof(DefaultExpressionVisitor).Assembly.GetType(
                    "System.Data.Entity.Core.Objects.ELinq.Funcletizer");
                var funcletizerFactoryMethod = funcletizerType.GetMethod("CreateQueryFuncletizer",
                    BindingFlags.Static | BindingFlags.NonPublic);
                var funcletizer = funcletizerFactoryMethod.Invoke(null, new[] { _objectContext });

                var converterType = typeof(DefaultExpressionVisitor).Assembly.GetType(
                    "System.Data.Entity.Core.Objects.ELinq.ExpressionConverter");
                var converter = Activator.CreateInstance(converterType,
                    BindingFlags.Instance | BindingFlags.NonPublic, null, new[] { funcletizer, linqExpression }, null);
                var convertMethod = converterType.GetMethod("Convert",
                    BindingFlags.NonPublic | BindingFlags.Instance);
                var result = (DbFilterExpression)convertMethod.Invoke(converter, null);

                var binding = current.Bind();
                var normalizer = new BindingNormalizer(binding);
                var output = result.Predicate.Accept(normalizer);
                current = binding.Filter(output);
            }
            return current;
        }

        private class BindingNormalizer : DefaultExpressionVisitor
        {
            private readonly DbExpressionBinding _binding;

            public BindingNormalizer(DbExpressionBinding binding)
            {
                _binding = binding;
            }

            public override DbExpression Visit(DbVariableReferenceExpression expression)
            {
                return _binding.VariableType.Variable(_binding.VariableName);
            }
        }

        public static Type GetClrType(IEnumerable<MetadataProperty> metadataProperties)
        {
            return (Type)GetAnnotation(metadataProperties, "http://schemas.microsoft.com/ado/2013/11/edm/customannotation:ClrType");
        }
        public static object GetAnnotation(IEnumerable<MetadataProperty> metadataProperties, string name)
        {
            var metadataProperty = metadataProperties.SingleOrDefault(p => p.Name.Equals(name, StringComparison.Ordinal));
            return metadataProperty == null ? null : metadataProperty.Value;
        }
    }
}