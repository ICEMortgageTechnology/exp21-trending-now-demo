# exp21-trending-now-demo
Experience 2021 Trending Now Demo.
This repository contains the source code for the demos shared in Experience21 breakout session: **Trending now: How to get started with APIs.** The code can be used as a reference to migrate from SDKs to Developer Connect APIs.

# Setup and Execution
1. Clone the repository or download the code.
2. Opent the solution **Exp21.DeveloperConnect.Demo.sln** in Visual Studio
3. Open App.config in **Exp21.DevConnect.Demo** project
4. Update the *appSettings* with values for your customer instance and test loan. You will need to add values for `HostUrl`, `UserName`, `ClientId`, `ClientSecret`, `LoanNumber` and `LoanFolder`. User name should be in format: `yourusername@encompass:YOURINSTANCEID`. `HostUrl` should be: `https://api.elliemae.com`.
5. Set **Exp21.DevConnect.Demo** as Startup project
6. Start Debugging. (Enter the password for the user when prompted.)

For more information on Developer Connect see: https://developer.elliemae.com/
