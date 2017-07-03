using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Ninject;
using EssentialTools.Models;
using Ninject.Web.Common;
namespace EssentialTools.Infrastructure
{
    public class NinjectDependencyResolver : IDependencyResolver
    {
        private IKernel kernel;
        public NinjectDependencyResolver(IKernel kernelParam)
        {
            kernel = kernelParam;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return kernel.TryGet(serviceType);
        }
        public IEnumerable<object> GetServices(Type serviceType)
        {
            return kernel.GetAll(serviceType);
        }
        private void AddBindings()
        {
          //  kernel.Bind<IValueCalculator>().To<LinqValueCalculator>().InRequestScope(); //每个请求创建单独实例，同一个请求中多个创建相同实例
            kernel.Bind<IValueCalculator>().To<LinqValueCalculator>().InSingletonScope(); //单例模式
            kernel.Bind<IDiscountHelper>().To<DefaultDiscountHelper>().WithPropertyValue("DiscountSize", 50M).WithConstructorArgument("deductionParam", 5M);
            kernel.Bind<IDiscountHelper>().To<FlexibleDiscountHelper>().WhenInjectedInto<LinqValueCalculator>();//当LinqValueCalculator调用时才使用
        }
    }
}