using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace middleWare
{
	public class data
	{
		public data() { }

		public string transactionGuid { get; set; }
		public string shortGuid { get; set; }
		public string requestTimestamp { get; set; }
		public string responseTimestamp { get; set; }
		public string verified { get; set; }
		public string center { get; set; }
		public string isException { get; set; }

		public Person person { get; set; }
		public string success { get; set; }
		public string code { get; set; }
		public string msg { get; set; }

        #region for returning status messages to client

		public Response error { get; set; }

        #endregion
    }

    public class Person
    {
		public string nationalId { get; set; }
		public string cardId { get; set; }
		public string cardValidFrom { get; set; }
		public string cardValidTo { get; set; }
		public string surname { get; set; }
		public string forenames { get; set; }
		public string nationality { get; set; }
		public string birthDate { get; set; }
		public string gender { get; set; }
		public string birthCountry { get; set; }
		public string birthDistrict { get; set; }
		public string birthRegion { get; set; }
		public string birthTown { get; set; }
		public Address[] addresses { get; set; }
		public Contact contact { get; set; }
		public BiometricFeed biometricFeed { get; set; }
		public Binary[] binaries { get; set; }
    }

	public class Binary
    {
		public string type { get; set; }
		public string dataType { get; set; }
		public string data { get; set; }
    }

	public class GPS
    {
		public string gpsName { get; set; }
    }

	public class Contact
    {
		public string email { get; set; }
		public Phone[] phoneNumbers { get; set; }
    }

	public class Phone
    {
		public string type { get; set; }
		public string phoneNumber { get; set; }
		public string network { get; set; }
    }

	public class BiometricFeed
    {
		public Face face { get; set; }
    }

	public class Face
    {
		public string dataType { get; set; }
		public string data { get; set; }
    }

	public class Address
    {
		public string type { get; set; }
		public string community { get; set; }
		public string postalCode { get; set; }
		public string region { get; set; }
		public string addressDigital { get; set; }
    }


	//PUSH Response payload
	public class PUSHPayload
    {
		public string customerNumber { get; set; }
		public string accountNumber { get; set; }
		public string accountName { get; set; }
		public string dateOfBirth { get; set; }
		public string mobileNumber { get; set; }
		public string idPhotoFront { get; set; }
		public string idPhotoBack { get; set; }

		public CallBack callbackData { get; set; }

    }

	public class CallBack
    {
		public string code { get; set; }
		public data data { get; set; }
		public bool success { get; set; }
    }

}

