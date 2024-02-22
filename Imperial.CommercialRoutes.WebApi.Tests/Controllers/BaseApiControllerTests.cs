using FluentAssertions;
using Moq;
using Serilog;
using System;
using Xunit;

namespace Imperial.CommercialRoutes.Application.Tests.Controllers
{
    /// <summary>
    /// Base class for all the API test controllers.
    /// </summary>
    /// <typeparam name="TController"> Controller type.</typeparam>
    public abstract class BaseApiControllerTests<TController> : IDisposable
    {
        #region Fields

        /// <summary>
        /// Gets the controller.
        /// </summary>
        protected abstract TController Controller { get; }

        /// <summary>
        /// Gets the logger mock.
        /// </summary>
        protected Mock<ILogger> LoggerMock { get; private set; }

        #endregion

        #region Setup and Clean Up

        /// <summary>
        /// Set up logic for the API controllers.
        /// </summary>
        protected void SetupBaseApiController()
        {
            LoggerMock = new Mock<ILogger>();
        }

        /// <summary>
        /// Tear down logic for the API controllers.
        /// </summary>
        protected void TierDownBaseApiController()
        {
            LoggerMock = null;
        }

        #endregion

        #region Test Methods

        /// <summary>
        /// Test that the controller <see cref="TController"/>
        /// inherits from the base API controller class <see cref="BaseApiController"/>.
        /// </summary>
        [Fact]
        public virtual void TestControllerIsDefined()
        {
            Controller.Should().NotBeNull();
        }

        public abstract void Dispose();

        #endregion
    }
}
