// <copyright file="ChatAppDispatcher.cs" company="Drastic Actions">
// Copyright (c) Drastic Actions. All rights reserved.
// </copyright>

using System;
using Drastic.Services;

namespace ThoughtPal.Services
{
    public class ChatAppDispatcher : IAppDispatcher
    {
        public bool Dispatch(Action action)
        {
            return Application.Current!.Dispatcher.Dispatch(action);
        }
    }
}