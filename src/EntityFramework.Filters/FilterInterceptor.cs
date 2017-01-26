namespace EntityFramework.Filters
{
    using System.Data.Entity.Core.Common.CommandTrees;
    using System.Data.Entity.Core.Metadata.Edm;
    using System.Data.Entity.Infrastructure.Interception;
    using System.Linq;

    public class FilterInterceptor : IDbCommandTreeInterceptor
    {
        public void TreeCreated(DbCommandTreeInterceptionContext interceptionContext)
        {
            if (interceptionContext.OriginalResult.DataSpace != DataSpace.CSpace) return;

            var queryCommand = interceptionContext.Result as DbQueryCommandTree;
            if (queryCommand == null) return;

            var context = interceptionContext.DbContexts.FirstOrDefault();
            if (context == null) return;

            interceptionContext.Result = new DbQueryCommandTree(
                queryCommand.MetadataWorkspace,
                queryCommand.DataSpace,
                queryCommand.Query.Accept(new FilterQueryVisitor(context)),
                validate: true,
                useDatabaseNullSemantics: queryCommand.UseDatabaseNullSemantics);
        }
    }
}
