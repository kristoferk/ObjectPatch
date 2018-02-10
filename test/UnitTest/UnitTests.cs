using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using ExampleApiWeb.Api.v1.Contracts;
using ExampleApiWeb.Framework.Models;
using Newtonsoft.Json.Linq;
using SimpleObjectPatch;
using Xunit;

namespace UnitTest
{
    public class UnitTests
    {
        [Fact]
        public void PatchCreateObject()
        {
            var input = new CustomerDto {
                Description = "Desc",
                Alias = "Alias",
                Notes = "Notes"
            };

            var dto = new PatchObject<CustomerDto>(input).Create();

            //Explicit patchable 
            Assert.True(dto.Description == "Desc");

            //Implicit patchable
            Assert.True(dto.Notes == "Notes");

            //Explicit NOT patchable and should not be changed
            Assert.True(dto.Alias == new CustomerDto().Alias);            
        }

        [Fact]
        public void PatchUpdateObject()
        {
            var original = new CustomerDto {
                Description = "OldDesc",
                Alias = "OldAlias",
                Notes = "OldNotes"
            };

            var input = new CustomerDto {
                Description = "Desc",
                Alias = "Alias",
                Notes = "Notes"
            };

            //PatchObject<CustomerDto>
            var newObject = new PatchObject<CustomerDto>(input).Patch(original);
            
            //Explicit patchable 
            Assert.True(newObject.Description == "Desc");

            //Implicit patchable
            Assert.True(newObject.Notes == "Notes");

            //Explicit NOT patchable and should not be changed
            Assert.True(newObject.Alias == "OldAlias");
        }

        [Fact]
        public void PatchSpecificProperties()
        {
            var original = new CustomerDto {
                Description = "OldDesc",
                Alias = "OldAlias",
                Notes = "OldNotes"
            };

            var input = new CustomerDto {
                Description = "Desc",
                Alias = "Alias",
                Notes = "Notes"
            };

            //PatchObject<CustomerDto>
            new PatchObject<CustomerDto>(input)
                .Patch(ref original,
                    c => c.Notes, 
                    c => c.Alias);

            //Explicit patchable but not specified
            Assert.True(original.Description == "OldDesc");

            //Implicit patchable and specified
            Assert.True(original.Notes == "Notes");

            //Explicit NOT patchable but specified
            Assert.True(original.Alias == "Alias");
        }

        [Fact]
        public void PatchForSpecificVersion()
        {
            var original = new CustomerDto {
                NewAttributeIn15 = string.Empty,
                RemovedAttributeIn15 = "Test"
            };

            var input = new CustomerDto {
                NewAttributeIn15 = "Test",
                RemovedAttributeIn15 = "Test2"
            };

            //NewAttribute
            var newObject = new PatchObject<CustomerDto>(input, 1.4M).Patch(original);
            Assert.True(newObject.NewAttributeIn15 == string.Empty);
            Assert.True(newObject.RemovedAttributeIn15 == "Test2");

            var newObject2 = new PatchObject<CustomerDto>(input, 1.5M).Patch(original);
            Assert.True(newObject2.NewAttributeIn15 == "Test");
            Assert.True(newObject2.RemovedAttributeIn15 == "Test2");

            var newObject3 = new PatchObject<CustomerDto>(input, 1.6M).Patch(original);
            Assert.True(newObject3.NewAttributeIn15 == "Test");
            Assert.True(newObject3.RemovedAttributeIn15 == "Test");
        }
    }
}