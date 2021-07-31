public class CustomResponseHeadersBuilder
{
	private readonly CustomResponseHeadersPolicy _policy = new CustomResponseHeadersPolicy ();

	public CustomResponseHeadersBuilder AddDefaultSecurePolicy ()
	{
		AddFrameOptionsDeny ();
		// AddXssProtectionBlock();
		//AddContentTypeOptionsNoSniff();
		// AddStrictTransportSecurityMaxAge();
		RemoveServerHeader ();

		return this;
	}

	public CustomResponseHeadersBuilder AddFrameOptionsDeny ()
	{
		// _policy.SetHeaders[FrameOptionsConstants.Header] = FrameOptionsConstants.Deny;
		return this;
	}

	public CustomResponseHeadersBuilder AddFrameOptionsSameOrigin ()
	{
		// _policy.SetHeaders[FrameOptionsConstants.Header] = FrameOptionsConstants.SameOrigin;
		return this;
	}

	public CustomResponseHeadersBuilder AddFrameOptionsSameOrigin (string uri)
	{
		// _policy.SetHeaders[FrameOptionsConstants.Header] = string.Format(FrameOptionsConstants.AllowFromUri, uri);
		return this;
	}

	public CustomResponseHeadersBuilder RemoveServerHeader ()
	{
		// _policy.RemoveHeaders.Add(ServerConstants.Header);
		return this;
	}

	public CustomResponseHeadersBuilder AddCustomHeader (string header, string value)
	{
		_policy.SetHeaders [header] = value;
		return this;
	}

	public CustomResponseHeadersBuilder RemoveHeader (string header)
	{
		_policy.RemoveHeaders.Add (header);
		return this;
	}

	public CustomResponseHeadersPolicy Build ()
	{
		return _policy;
	}
}