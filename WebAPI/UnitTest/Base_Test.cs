using Application.Repositories;
using Infrastructure.Data;
using Moq;

namespace UnitTest
{
    public abstract class Base_Test
    {
        protected Mock<DataContext> _mockContext;
        protected Mock<IUnitOfWork> _mockUnitOfWork;

        public Base_Test()
        {
            _mockContext = new Mock<DataContext>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            Setup();
        }

        protected abstract void Setup();
    }
}
