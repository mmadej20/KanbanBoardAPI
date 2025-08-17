using KanbanBoard.Application.Members.Errors;
using KanbanBoard.Infrastructure.Services;
using KanbanBoard.Tests.DatabaseFixture;
using Shouldly;
using TUnit.Core.Logging;

namespace KanbanBoard.Tests
{
    [ClassDataSource(typeof(KanbanDatabaseFixture))]
    public class MembersTest
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.
        private MemberService _memberService;
        private DefaultLogger _output;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider adding the 'required' modifier or declaring as nullable.

        [Before(HookType.Test)]
        public void Initialize()
        {
            _memberService = ServicesWithFixtureDatabase.GetMemberService();
            _output = TestContext.Current!.GetDefaultLogger();
        }

        [Test]
        public async Task MemberShouldBeAddedToDatabase()
        {
            var memberName = "New Member";
            var email = "newmember@looktuo.com";

            var result = await _memberService.AddMember(memberName, email);

            result.IsSuccess.ShouldBe(true);
        }

        [Test]
        public async Task ShouldReturnEmailAlreadyInUse()
        {
            var memberName = "user2";
            var email = "user1@liamg.com";

            var result = await _memberService.AddMember(memberName, email);

            result.IsFailure.ShouldBe(true);
            result.Error.ShouldBeEquivalentTo(MemberServiceErrors.EmailAlreadyInUse(email));
        }

        [Test]
        [Arguments("User1Updated")]
        public async Task MemberShouldBeUpdatedWithNewName(string newName)
        {
            await _memberService.UpdateMember(1, newName);

            var updatedMember = await _memberService.GetMemberById(1);
            updatedMember.IsSuccess.ShouldBe(true);

            _output.LogInformation($"Updated member name: {updatedMember.Value.MemberName}");
            updatedMember.Value.MemberName.ShouldBe(newName);
        }

        [Test]
        public async Task MemberShouldBeRemoved()
        {
            await _memberService.DeleteMember(2);

            var deletedMember = await _memberService.GetMemberById(2);
            deletedMember.IsFailure.ShouldBe(true);
        }
    }
}
