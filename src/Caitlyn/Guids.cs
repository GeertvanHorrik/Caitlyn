// Guids.cs
// MUST match guids.h
using System;

namespace Caitlyn
{
    static class GuidList
    {
        public const string guidCaitlynPkgString = "9106c2a4-0c2d-4064-9c05-b0d3e408d905";
        public const string guidCaitlynCmdSetString = "0a0015ae-880a-4a1e-9920-b101d0cdd461";

        public static readonly Guid guidCaitlynCmdSet = new Guid(guidCaitlynCmdSetString);
    };
}