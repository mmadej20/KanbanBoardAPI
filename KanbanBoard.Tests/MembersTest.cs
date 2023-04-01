using DataAccess.Models;
using KanbanBoard.Services;
using KanbanBoard.Tests.DatabaseFixture;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace KanbanBoard.Tests
{
    public class MembersTest : IClassFixture<KanbanDatabaseFixture>
    {
        private readonly MemberService _memberService;
        private readonly ITestOutputHelper _output;

        public MembersTest(ITestOutputHelper output)
        {
            _memberService = ServicesWithFixtureDatabase.GetMemberService();
            _output = output;
        }

        [Fact]
        public async Task MemberShouldBeAddedToDatabase()
        {
            var memberName = "New Member";
            var email = "newmember@looktuo.com";

            var result = await _memberService.AddMember(memberName, email);

            Assert.True(result.IsSuccesfull);
        }

        [Fact]
        public async Task ShouldReturnEmailAlreadyInUse()
        {
            var memberName = "user2";
            var email = "user1@liamg.com";

            var result = await _memberService.AddMember(memberName, email);

            Assert.Contains($"Email '{email}' is already in use!", result.Message);
        }

        [Theory]
        [InlineData("User1Updated")]
        public async Task MemberShouldBeUpdatedWithNewName(string newName)
        {
            await _memberService.UpdateMember(1, newName);

            var updatedMember = await _memberService.GetMemberById(1);

            _output.WriteLine($"Updated member name: {updatedMember.MemberName}");
            Assert.Equal(updatedMember.MemberName, newName);
        }

        [Fact]
        public async Task MemberShouldBeRemoved()
        {
            await _memberService.DeleteMember(2);

            var deletedMember = await _memberService.GetMemberById(2);

            Assert.Null(deletedMember);
        }
    }
}
