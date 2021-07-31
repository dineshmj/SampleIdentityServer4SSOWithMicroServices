using FourWallsInc.Entity.SSO;

namespace CMS.IDP.App.Business
{
	/// <summary>
	/// Abstracts the behavior of working with the users' data
	/// in the LoB application's DB.
	/// </summary>
	public interface ICmsAuthenticationBizFacade
	{
		#region Methods.

		/// <summary>
		/// Validates the specified credentials of the signing in user.
		/// </summary>
		/// <param name="loginId">The login ID.</param>
		/// <param name="password">The password.</param>
		/// <returns></returns>
		bool ValidateCredentials (string loginId, string password);

		/// <summary>
		/// Gets a user matching to the specified login ID.
		/// </summary>
		/// <param name="loginId">The login ID.</param>
		/// <returns></returns>
		LobApplicationUser FindUserByLoginId (string loginId);

		#endregion
	}
}
