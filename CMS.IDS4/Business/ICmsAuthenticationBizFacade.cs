using FourWallsInc.Entity.SSO;

namespace CMS.IDP.App.Business
{
	// Abstracts the behavior of working with the users' data
	// in the LoB application's DB.
	public interface ICmsAuthenticationBizFacade
	{
		// Validates the specified credentials of the signing in user.
		bool ValidateCredentials (string loginId, string password);

		// Gets a user matching to the specified login ID.
		LobApplicationUser FindUserByLoginId (string loginId);
	}
}
