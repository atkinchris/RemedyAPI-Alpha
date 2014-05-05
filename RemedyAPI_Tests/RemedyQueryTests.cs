using Microsoft.VisualStudio.TestTools.UnitTesting;
using RemedyAPI;
using System;

namespace RemedyAPI_Tests {
    [TestClass]
    public class RemedyQueryTests {

        private string testUsername = "user";
        private string testPassword = "password";
        private string invalidUsername = "$$";

        [TestMethod]
        public void Constructor_Default() {
            var remedy = new RemedyQuery( testUsername, testPassword );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ),
            "A username of null was inappropriately allowed." )]
        public void Constructor_NullUsername() {
            var remedy = new RemedyQuery( null, testPassword );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ),
            "An empty username was inappropriately allowed." )]
        public void Constructor_EmptyUsername() {
            var remedy = new RemedyQuery( string.Empty, testPassword );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ),
            "A password of null was inappropriately allowed." )]
        public void Constructor_NullPassword() {
            var remedy = new RemedyQuery( testUsername, null );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ),
            "An empty password was inappropriately allowed." )]
        public void Constructor_EmptyPassword() {
            var remedy = new RemedyQuery( testUsername, string.Empty );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ),
            "A username of null was inappropriately allowed." )]
        public void SetUsername_Null() {
            var remedy = new RemedyQuery( testUsername, testPassword );
            remedy.SetUsername( null );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ),
            "An empty username was inappropriately allowed." )]
        public void SetUsername_Empty() {
            var remedy = new RemedyQuery( testUsername, testPassword );
            remedy.SetUsername( string.Empty );
        }

        [TestMethod]
        [ExpectedException( typeof( ArgumentException ),
            "An empty username was inappropriately allowed." )]
        public void SetUsername_Invalid() {
            var remedy = new RemedyQuery( testUsername, testPassword );
            remedy.SetUsername( invalidUsername );
        }
    }
}
