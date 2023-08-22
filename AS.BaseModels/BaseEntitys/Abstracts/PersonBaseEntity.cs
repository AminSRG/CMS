namespace AS.BaseModels.BaseEntitys.Abstracts
{
    public class PersonBaseEntity : EntityID
    {
        public PersonBaseEntity(string fullName, byte age, 
                                string phoneNumber, string email) : base()
        {
            FullName = fullName;
            Age = age;
            PhoneNumber = phoneNumber;
            Email = email;
        }

        public string FullName { get; set; }
        public byte Age { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

    }
}
