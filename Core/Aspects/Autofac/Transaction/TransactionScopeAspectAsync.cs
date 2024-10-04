using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.EntityFrameworkCore;
using System.Transactions;

namespace Core.Aspects.Autofac.Transaction
{
    public class TransactionScopeAspectAsync : MethodInterception
    {
        private readonly Type _dbContextType;

        public async Task InterceptDbContext(IInvocation invocation)
        {
            var db = ServiceTool.ServiceProvider.GetService(_dbContextType) as DbContext;
            using var transactionScope = await db.Database.BeginTransactionAsync();
            try
            {
                invocation.Proceed();
                await transactionScope.CommitAsync();
            }
            finally
            {
                await transactionScope.RollbackAsync();
            }
        }

        public override void Intercept(IInvocation invocation)
        {
            var task = InterceptAsync(invocation);
            task.GetAwaiter().GetResult();
        }

        private async Task InterceptAsync(IInvocation invocation)
        {
            using var tx = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
            await ProceedInvocationAsync(invocation);
            tx.Complete();
        }

        private async Task ProceedInvocationAsync(IInvocation invocation)
        {
            invocation.Proceed();
            if (invocation.ReturnValue is Task returnValueTask)
            {
                await returnValueTask;

            }
            else if (invocation.ReturnValue is ValueTask returnValueValueTask)
            {
                await returnValueValueTask;
            }
        }
    }
}
