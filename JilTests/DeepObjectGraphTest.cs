using System;
using System.IO;
using System.Text;
using Jil;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JilDeepGraphTest
{
    [TestClass]
    public class DeepObjectGraphTests
    {
        [TestMethod]
        public void DeepObjectGraphShouldNotBeEmpty()
        {
            // given
            var model = new UserCredentialsUpdateResponse()
            {
                User =
                    new UserDto()
                    {
                        DateOfBirth = new DateTime(1980, 01, 01),
                        CreatedAtUtc = new DateTime(2014, 01, 01, 08, 00, 00),
                        UserId = 1,
                        Username = "SomeUsername"
                    }
            };

            //When
            var stringBuilder = new StringBuilder();
            using (var stringOutput = new StringWriter(stringBuilder))
            {
                JSON.Serialize(model, stringOutput, Options.IncludeInherited);
            }
            var serializedJsonAsString = stringBuilder.ToString();

            // round tripping
            var deserializedRoundtrippedObject = JSON.Deserialize(serializedJsonAsString, typeof(UserCredentialsUpdateResponse), Options.IncludeInherited);
            var deserializedRoundtrippedInstance = deserializedRoundtrippedObject as UserCredentialsUpdateResponse;

            // Then
            Assert.AreNotEqual("{}", serializedJsonAsString);
            Assert.IsNotNull(deserializedRoundtrippedInstance);
            Assert.IsInstanceOfType(deserializedRoundtrippedInstance, typeof(UserCredentialsUpdateResponse));

            Assert.IsNotNull(deserializedRoundtrippedInstance.User); // << this fails

            Assert.AreEqual(deserializedRoundtrippedInstance.User.CreatedAtUtc.ToUniversalTime(), model.User.CreatedAtUtc.ToUniversalTime());
            Assert.AreEqual(deserializedRoundtrippedInstance.User.DateOfBirth.ToUniversalTime(), model.User.DateOfBirth.ToUniversalTime());
            Assert.AreEqual(deserializedRoundtrippedInstance.User.UserId, model.User.UserId);
            Assert.AreEqual(deserializedRoundtrippedInstance.User.Username, model.User.Username);
        }
    }

    public class UserCredentialsUpdateResponse : UserResponse
    {
        public UserCredentialsUpdateResponse()
        {
        }
    }

    public class UserResponse : Response
    {
        public UserResponse()
        {

        }

        public UserDto User { get; set; }
    }

    public class UserDto : CreatedAtDto
    {
        public UserDto()
        {
        }

        public DateTime DateOfBirth { get; set; }

        public long UserId { get; set; }

        public string Username { get; set; }
    }

    #region Base Classes
    public abstract class CreatedAtDto
    {
        public virtual DateTime CreatedAtUtc { get; set; }
    }

    public abstract class Response
    {

    }
    #endregion Base Classes
}