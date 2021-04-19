using Microsoft.Extensions.Logging;
using NHibernate;
using NHibernate.SqlCommand;

namespace Demo.Models
{
    public class SqlLoggingInterceptor : EmptyInterceptor
    {
        private readonly ILogger<SqlLoggingInterceptor> logger;
 
        public SqlLoggingInterceptor(ILogger<SqlLoggingInterceptor> logger)
        {
            this.logger = logger;
        }
 
        public override SqlString OnPrepareStatement(SqlString sql)
        {
            if (logger != null)
            {
                logger.LogDebug("nhibernate sql: {Sql}", sql);
            }
 
            return base.OnPrepareStatement(sql);
        }
    }
}