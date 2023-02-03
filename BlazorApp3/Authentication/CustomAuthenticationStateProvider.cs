﻿using System.Security.Claims;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server.ProtectedBrowserStorage;

namespace BlazorApp3.Authenticaion;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
	private readonly ProtectedSessionStorage _sessionStorage;
	private ClaimsPrincipal _anonymous = new ClaimsPrincipal(new ClaimsIdentity());

	public CustomAuthenticationStateProvider(ProtectedSessionStorage sessionStorage)
	{
		_sessionStorage = sessionStorage;
	}

	public override async Task<AuthenticationState> GetAuthenticationStateAsync()
	{
		try
		{
				var userSessionResult = await _sessionStorage.GetAsync<UserSession>("UserSession");
         		var userSession = userSessionResult.Success
         			? userSessionResult.Value
         			: null;
         
         		if (userSession is null)
         			return await Task.FromResult(new AuthenticationState(_anonymous));
         
         		var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
         		{
         			new(ClaimTypes.Name, userSession.UserName),
         			new(ClaimTypes.Role, userSession.Role)
         		}, "CustomAuth"));
         
         		return await Task.FromResult(new AuthenticationState(claimsPrincipal));
		}
		catch
		{
			return await Task.FromResult(new AuthenticationState(_anonymous));
		}
	
	}

	public async Task UpdateAuthenticationState(UserSession? userSession)
	{
		ClaimsPrincipal claimsPrincipal;
		
		if (userSession != null)
		{
			await _sessionStorage.SetAsync("UserSession", userSession);
			claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>
			{
				new(ClaimTypes.Name, userSession.UserName),
				new(ClaimTypes.Role, userSession.Role)
			}));
		}
		else
		{
			await _sessionStorage.DeleteAsync("UserSession");
			claimsPrincipal = _anonymous;
		}
		
		NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
	}
}