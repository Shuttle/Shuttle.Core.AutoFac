﻿using System;
using System.Collections.Generic;
using Autofac;
using Shuttle.Core.Infrastructure;

namespace Shuttle.Core.Autofac
{
	public class AutofacComponentRegistry : ComponentRegistry
	{
		private readonly ContainerBuilder _containerBuilder;

		public AutofacComponentRegistry(ContainerBuilder containerBuilder)
		{
			Guard.AgainstNull(containerBuilder, "containerBuilder");

			_containerBuilder = containerBuilder;
		}

		public override IComponentRegistry Register(Type dependencyType, Type implementationType, Lifestyle lifestyle)
		{
			Guard.AgainstNull(dependencyType, "dependencyType");
			Guard.AgainstNull(implementationType, "implementationType");

			base.Register(dependencyType, implementationType, lifestyle);

			try
			{
				switch (lifestyle)
				{
					case Lifestyle.Transient:
					{
						_containerBuilder.RegisterType(implementationType).As(dependencyType).InstancePerDependency();

						break;
					}
					default:
					{
						_containerBuilder.RegisterType(implementationType).As(dependencyType).SingleInstance();

						break;
					}
				}
			}
			catch (Exception ex)
			{
				throw new TypeRegistrationException(ex.Message, ex);
			}

			return this;
		}

		public override IComponentRegistry RegisterCollection(Type dependencyType, IEnumerable<Type> implementationTypes,
			Lifestyle lifestyle)
		{
			Guard.AgainstNull(dependencyType, "dependencyType");
			Guard.AgainstNull(implementationTypes, "implementationType");

			base.RegisterCollection(dependencyType, implementationTypes, lifestyle);

			try
			{
				switch (lifestyle)
				{
					case Lifestyle.Transient:
					{
						foreach (var type in implementationTypes)
						{
							_containerBuilder.RegisterType(type).As(dependencyType).InstancePerDependency();
						}

						break;
					}
					default:
					{
						foreach (var type in implementationTypes)
						{
							_containerBuilder.RegisterType(type).As(dependencyType).SingleInstance();
						}

						break;
					}
				}
			}
			catch (Exception ex)
			{
				throw new TypeRegistrationException(ex.Message, ex);
			}

			return this;
		}

		public override IComponentRegistry Register(Type dependencyType, object instance)
		{
			Guard.AgainstNull(dependencyType, "dependencyType");
			Guard.AgainstNull(instance, "instance");

			base.Register(dependencyType, instance);

			try
			{
				_containerBuilder.RegisterInstance(instance).As(dependencyType);
			}
			catch (Exception ex)
			{
				throw new TypeRegistrationException(ex.Message, ex);
			}

			return this;
		}
	}
}