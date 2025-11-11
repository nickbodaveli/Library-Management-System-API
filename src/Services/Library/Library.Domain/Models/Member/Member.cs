using Domain.Abstractions.Abstractions;
using Library.Domain.Events;
using Library.Domain.Exceptions;
using Library.Domain.ValueObjects.Member;

namespace Library.Domain.Models.Member
{
    public class Member : Aggregate<MemberId>
    {
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public bool IsActive { get; private set; }

        private Member() { }

        public Member(
            MemberId id,
            string firstName,
            string lastName,
            string email)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            IsActive = true; 
        }

        public static Member Create(string firstName, string lastName, string email)
        {
            var member = new Member(
                MemberId.New(),
                firstName,
                lastName,
                email
            );

            member.AddDomainEvent(new MemberCreatedDomainEvent(
                member.Id,
                member.FirstName,
                member.LastName,
                member.Email));

            return member;
        }

        public void Deactivate()
        {
            if (!IsActive) 
            {
                throw new DomainException("Member is already inactive."); 
            }

            IsActive = false; 

            AddDomainEvent(new MemberStatusUpdatedDomainEvent(this.Id.Value, this.IsActive));
        }

        public void UpdateInfo(string firstName, string lastName, string email, bool isActive)
        {
            if (string.IsNullOrWhiteSpace(email)) 
            {
                throw new DomainException("Member email cannot be empty."); 
            }

            this.FirstName = firstName; 
            this.LastName = lastName; 
            this.Email = email; 
            this.IsActive = isActive;

            AddDomainEvent(new MemberUpdatedDomainEvent(
                this.Id.Value,
                this.FirstName,
                this.LastName,
                this.Email,
                this.IsActive
            ));
        }

        public void ApplyProjectionUpdate(bool isActive)
        {
            this.IsActive = isActive;
        }

        public void ApplyDetailsProjectionUpdate(string firstName, string lastName, string email)
        {
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Email = email;
        }
    }
}
