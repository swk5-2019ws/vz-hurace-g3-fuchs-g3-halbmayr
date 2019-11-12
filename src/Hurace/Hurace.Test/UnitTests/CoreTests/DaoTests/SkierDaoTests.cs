using Hurace.Core.Dal;
using Hurace.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Xunit;

namespace Hurace.Test.UnitTests.CoreTests.DaoTests
{
    public class SkierDaoTests : IDisposable
    {
        #region Transaction Boilerplate Code

        private bool disposed = false;

        private readonly TransactionScope transactionScope;

        public SkierDaoTests()
        {
            transactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Protected implementation of Dispose pattern.
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
                transactionScope.Dispose();

            disposed = true;
        }

        #endregion

        [Fact]
        public async Task TestGetAll()
        {
            var skierDao = DaoFactory.CreateSkierDao();

            var skiers = await skierDao.GetAllAsync();

            Assert.Equal(521, skiers.Count());
        }
    }
}
