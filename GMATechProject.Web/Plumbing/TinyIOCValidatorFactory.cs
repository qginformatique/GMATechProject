using System;
using TinyIoC;
using FluentValidation;

namespace ABC.Domain
{
	public class TinyIOCValidatorFactory : ValidatorFactoryBase
	{
		private readonly TinyIoCContainer _Container;
		
		public TinyIOCValidatorFactory (TinyIoCContainer container)
		{
			this._Container = container;
		}

		public override FluentValidation.IValidator CreateInstance (Type validatorType)
		{
			if (_Container.CanResolve (validatorType)) {
				return (IValidator)_Container.Resolve (validatorType);
			}
			
			return null;
		}
	}
}

