﻿using System;
using App.Template.XForms.Core.Contracts;
using MvvmCross.Platform;
using Newtonsoft.Json;
using Validation;

namespace App.Template.XForms.Core.Utils.Auth
{
    /// <summary>
    /// This class represents a serialized access token. We use this class to automatically deserialize the 
    /// result of an OAuth token response.
    /// </summary>
    public class AccessTokenResponse
    {
        /// <summary>
        /// Gets or sets the access token.
        /// </summary>
        /// <value>
        /// The access token.
        /// </value>
        [JsonProperty(PropertyName = "access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets the refresh token.
        /// </summary>
        /// <value>
        /// The refresh token.
        /// </value>
        [JsonProperty(PropertyName = "refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// Gets or sets the token type.
        /// </summary>
        /// <value>
        /// The token type.
        /// </value>
        [JsonProperty(PropertyName = "token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// Gets or sets the number of seconds the token has before it expires. If this value is <c>null</c>,
        /// the token will never expire.
        /// </summary>
        /// <value>
        /// The number of seconds the token has before it expires, or <c>null</c> if it never expires.
        /// </value>
        /// 
        [JsonProperty(PropertyName = "expires_in")]
        public int? ExpiresIn { get; set; }

        /// <summary>
        /// Gets or sets the scope of the token.
        /// </summary>
        /// <value>
        /// The scope.
        /// </value>
        [JsonProperty(PropertyName = "scope")]
        public string Scope { get; set; }

        private DateTime? ExpirationDate => ExpiresIn == null ? (DateTime?)null : Mvx.Resolve<INow>().DateNow.AddSeconds(Convert.ToInt32(ExpiresIn));

        /// <summary>
        /// Convert this instance to a <see cref="Auth.AccessToken"/> instance.
        /// </summary>
        /// <returns>An <see cref="Auth.AccessToken"/> that represents this instance.</returns>
        public AccessToken ToAccessToken()
        {
            Verify.Operation(AccessToken != null, "The \"access_token\" property must not be null.");
            Verify.Operation(TokenType != null, "The \"token_type\" property must not be null.");

            return new AccessToken(AccessToken, TokenType, Scope, ExpirationDate, RefreshToken);
        }
    }
}