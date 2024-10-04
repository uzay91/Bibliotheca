using Castle.DynamicProxy;
using Core.Utilities.Interceptors;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.CrossCuttingConcerns.Validation.FluentValidation;

namespace Core.Aspects.Autofac.Validation
{
    public class ValidationAspect : MethodInterception
    {
        private Type _validatorType;

        public ValidationAspect(Type validatorType)
        {
            if (!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new Exception("Wrong Validation Type");
            }

            _validatorType = validatorType;
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            var objType = _validatorType.BaseType.GetGenericArguments()[0];
            var objList = invocation.Arguments.Where(o => o.GetType() == objType);

            foreach (var obj in objList)
            {
                ValidationTool.Validate(validator, obj);
            }
        }
    }
}
