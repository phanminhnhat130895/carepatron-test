using Application.Repositories;
using Domain.Entities;
using Infrastructure.Data;
using Moq;

namespace UnitTest
{
    public abstract class BaseService_Test
    {
        protected Mock<DataContext> _mockContext;
        protected Mock<IUnitOfWork> _mockUnitOfWork;

        public BaseService_Test()
        {
            _mockContext = new Mock<DataContext>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            Setup();
        }

        protected abstract void Setup();
    }
}
