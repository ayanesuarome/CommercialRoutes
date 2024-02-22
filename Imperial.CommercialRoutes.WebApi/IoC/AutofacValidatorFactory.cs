using Autofac;
using FluentValidation;
using System;

namespace Imperial.CommercialRoutes.WebApi.IoC
{
    /// <summary>
    /// Autofac validator factory class which resolves the actual validator
    /// from the container when the FluentValidation library requests it.
    /// </summary>
    public class AutofacValidatorFactory : ValidatorFactoryBase
    {
        #region Fields

        private readonly IComponentContext _context;

        #endregion

        #region Constructor

        /// <summary>
        /// Initialize a new instance of <see cref="AutofacValidatorFactory" />.
        /// </summary>
        /// <param name="context">Autofac context</param>
        public AutofacValidatorFactory(IComponentContext context)
        {
            _context = context;
        }

        #endregion

        #region Overrides

        public override IValidator CreateInstance(Type validatorType)
        {
            object instance;

            if (_context.TryResolve(validatorType, out instance))
            {
                var validator = instance as IValidator;

                return validator;
            }

            return null;
        }

        #endregion
    }
}