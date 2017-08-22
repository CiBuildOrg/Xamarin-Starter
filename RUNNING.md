# Making the Xamarin template work with the web api template 

This contains some instructions on how to link the xamarin app to the [Web api boilerplate](https://github.com/CiBuildOrg/WebApi-Boilerplate) 

## Getting Started

Install web api boilerplate (following the instructions on [README](https://github.com/CiBuildOrg/WebApi-Boilerplate/blob/master/README.md))

Install the api in IIS and give your IIS application pool an elevated [identity](https://technet.microsoft.com/en-us/library/cc771170(v=ws.10).aspx) or change your sql connection to include a proper username and password. 

Access the web api service from the xamarin simulator. 

### Prerequisites

You need Visual Studio 2017 to run both the xamarin app and the webapi server.
You will also need sql server and IIS.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details
