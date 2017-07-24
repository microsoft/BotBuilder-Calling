// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Bot Framework: http://botframework.com
// 
// Bot Builder SDK GitHub:
// https://github.com/Microsoft/BotBuilder
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using Microsoft.Bot.Builder.Calling.ObjectModel.Misc;
using Newtonsoft.Json;

namespace Microsoft.Bot.Builder.Calling.ObjectModel.Contracts
{
    /// <summary>
    /// This is the action which customers can specify to indicate that the server call agent should transfer established
    /// call. The transfer can be Attended - meaning if the transfer fails the agent is able to still interact with caller.
    /// If transfer succeeds the call is automatically hang up or it can be Consultative - meaning the transfer to the target
    /// will replace an existing call of agent with the target.
    /// </summary>
    [JsonObject(MemberSerialization.OptOut)]
    public class Transfer : ActionBase
    {
        /// <summary>
        /// The Skype identifier of transfer target.
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public Participant Target { get; set; }

        /// <summary>
        /// Gets or sets the replaces context of the existing call with the target.
        /// If this is set a consultative transfer takes place otherwise an attended transfer.
        /// </summary>
        /// <value>
        /// The replaces context.
        /// </value>
        [JsonProperty(Required = Required.Default)]
        public string ReplacesContext { get; set; }

        public Transfer()
        {
            this.Action = ValidActions.TransferAction;
        }

        public override void Validate()
        {
            base.Validate();
            Utils.AssertArgument(this.Target != null, "Target cannot be null");
            this.Target.Validate();
            Utils.AssertArgument(this.Target.Identity.StartsWith("8:")
                || this.Target.Identity.StartsWith("29:"), "Target identity has to be 8:* or 29:* Skype Identity");
            Utils.AssertArgument(this.Target.Originator == false, "Target originator has to be set to false");
        }
    }
}
