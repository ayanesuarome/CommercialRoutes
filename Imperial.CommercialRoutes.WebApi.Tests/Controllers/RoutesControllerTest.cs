using Imperial.CommercialRoutes.Application.Services;
using Imperial.CommercialRoutes.Application.Tests.Controllers;
using Imperial.CommercialRoutes.WebApi.Controllers;
using Moq;

namespace Imperial.CommercialRoutes.WebApi.Tests.Controllers
{
    /// <summary>
    /// Tests class to tests the <see cref="RoutesControllerTest"/> implementations.
    /// </summary>
    public class RoutesControllerTest : BaseApiControllerTests<RoutesController>
    {
        #region Fields

        private RoutesController _controller;
        private Mock<ICommercialRoutesService> _serviceMock;

        /// <summary>
        /// Gets test controller.
        /// </summary>
        protected override RoutesController Controller => _controller;

        #endregion

        #region Setup and Cleanup

        /// <summary>
        /// Initializes a new instance of the class <see cref="RoutesControllerTest"/>.
        /// </summary>
        public RoutesControllerTest()
        {
            SetupBaseApiController();

            _serviceMock = new Mock<ICommercialRoutesService>();

            _controller = new RoutesController(_serviceMock.Object);
        }

        /// <summary>
        /// Releases unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            TierDownBaseApiController();
            _controller = null;
            _serviceMock = null;
        }

        #endregion
    }
}
