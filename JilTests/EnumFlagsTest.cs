using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jil;
using JilTests.Hashbrowns.Shared.Enums;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace JilTests
{
    using System;

    namespace Hashbrowns.Shared.Enums
    {
        [Flags]
        public enum APIRoles : byte
        {
            None = 0,
            Trial = 1,
            Tier1 = Trial | 2,
            Tier2 = Tier1 | 4,
            Tier3 = Tier2 | 8,
            AllRoles = 255
        }
    }

    [TestClass]
    public class EnumFlagsTest
    {
        [TestMethod]
        public void TestEnumFlagsSerialize()
        {
            APIRoles roles = APIRoles.AllRoles;

            string data = JSON.Serialize(roles);

            Assert.AreEqual(data, "\"Trial,Tier1,Tier2,Tier3,AllRoles\"");
        }

        [TestMethod]
        public void TestEnumFlagsDeserialize()
        {
            APIRoles roles;

            string data = "\"Trial,Tier1,Tier2,Tier3,AllRoles\"";

            roles = JSON.Deserialize<APIRoles>(data);

            Assert.AreEqual(roles, APIRoles.AllRoles);
        }

        [TestMethod]
        public void TestEnumFlagsDeserialize2()
        {
            APIRoles roles;

            string data = "\"AllRoles\"";

            roles = JSON.Deserialize<APIRoles>(data);

            Assert.AreEqual(roles, APIRoles.AllRoles);
        }

        [TestMethod]
        public void TestEnumFlagsDeserialize3()
        {
            APIRoles roles;

            string data = "\"None, Tier2\"";

            roles = JSON.Deserialize<APIRoles>(data);

            Assert.AreEqual(roles, APIRoles.Tier2);
        }
    }
}
