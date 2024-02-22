using Serilog;
using System;

namespace Imperial.CommercialRoutes.Infrastructure.Logging
{
    /// <summary>
    /// Logger provider.
    /// </summary>
    public class LoggerProvider : IDisposable
    {
        #region Fields

        readonly ILogger _logger;
        readonly Action _releaseAction;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the class <see cref="LoggerProvider"/>.
        /// </summary>
        /// <param name="logger">Logger instance.</param>
        public LoggerProvider(ILogger logger = null)
        {
            _logger = logger ?? Log.Logger;

            if (logger == null)
            {
                _releaseAction = () => { Log.CloseAndFlush(); };
            }
            else
            {
                _releaseAction = () => { (_logger as IDisposable)?.Dispose(); };
            }
        }

        #endregion

        #region Methods

        public ILogger GetLogger()
        {
            return _logger;
        }

        #endregion

        #region Dispose

        void IDisposable.Dispose()
        {
            _releaseAction();
        }

        #endregion
    }
}