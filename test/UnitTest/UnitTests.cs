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
            var newObject = new PatchObject<CustomerDto>(input).Patch(original, c => c.Notes, c => c.Alias);

            //Explicit patchable but not specified
            Assert.True(newObject.Description == "OldDesc");

            //Implicit patchable and specified
            Assert.True(newObject.Notes == "Notes");

            //Explicit NOT patchable but specified
            Assert.True(newObject.Alias == "Alias");
        }
    }
}