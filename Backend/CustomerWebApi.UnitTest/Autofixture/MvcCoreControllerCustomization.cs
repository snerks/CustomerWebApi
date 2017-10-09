using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Http.Features;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Xunit;
using FluentAssertions;
using System;
using Moq;
using Ploeh.AutoFixture;
using System.Net.Http;
using Ploeh.AutoFixture.AutoMoq;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;

namespace CustomerWebApi.UnitTest.AutoFixture
{
    public class MvcCoreControllerCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => fixture.Behaviors.Remove(b));

            fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            fixture.Register<IModelMetadataProvider>(() => new EmptyModelMetadataProvider());
        }
    }
}
