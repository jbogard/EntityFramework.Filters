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
            if (interceptionContext.OriginalResult.DataSpace == DataSpace.CSpace)
            {
                var queryCommand = interceptionContext.Result as DbQueryCommandTree;
                if (queryCommand != null)
                {
                    var context = interceptionContext.DbContexts.FirstOrDefault();
                    if (context != null)
                    {
                        var newQuery =
                            queryCommand.Query.Accept(new FilterQueryVisitor(context));
                        interceptionContext.Result = new DbQueryCommandTree(
                            queryCommand.MetadataWorkspace, queryCommand.DataSpace, newQuery);
                    }
                }
            }
        }
    }
}
