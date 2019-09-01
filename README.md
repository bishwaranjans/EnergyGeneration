# Energy Generation Report Documentation
The EnergyGeneration application has been designed inline with SOA and Domain Driven Design(DDD) to provide end user a console application to process energy generation report and providing simulatd output file in real-time and dynamic environment.

## Implementation Requirement
An XML file containing generator data (see accompanying file GenerationReport.xml) is produced and provided as input into an input folder on a regular basis. 
The solution is required to automatically pick up the received XML file as soon as it is placed in the input folder (location of input folder is set in the Application app.config file), perform parsing and data processing as appropriate to achieve the following:
1.	It is required to calculate and output the **Total Generation Value** for each generator.
2.	It is required to calculate and output the generator with the **highest Daily Emissions for each day along with the emission value**.
3.	It is required to calculate and output **Actual Heat Rate for each coal generator**. 
The output should be a single XML file in the format as specified by an example accompanying file GenerationOutput.xml into an output folder (location of output folder is set in the Application app.config file).  


## Solution Architecture

DDD approch has been used for designing the architecture of the solution by clearly segregating the each responsibility with clear structure.
 - **EnergyGeneration.Console** : It is user interface of our solution signifying starting of the application and further processing and report generation. This is the entry block of our program.
 - **EnergyGeneration.Domain** : Responsible for representing concepts of the business, information about the business situation, and business rules. State that reflects the business situation is controlled and used here, even though the technical details of storing it are delegated to the infrastructure. This layer is the heart of our solution.
 - **EnergyGeneration.Infrastructure** : Responsible for how the data that is initially held in domain entities (in memory) or another persistent store. It contains all our parsing logic along wth validation.It also contains the logic for our energy calculation from the generator and the required output core logic.
 - **EnergyGeneration.UnitTests** : Responsible for mirroring the structure of the code under test. **ToDO**
 
 ![alt text](https://github.com/bishwaranjans/EnergyGeneration/blob/master/Documentation/EnergyGenerationReport.PNG)
 
 ## Design Patterns
 
Facade and Factory design patterns has been incorporated to design the application. The primary focus was to accommodate multiple parser into the application. Currently it is supporting XML parsing and later on it provides the extensibility to support any other parsing like CSV or EXcel etc. Basic SOLID design patterns has been followed wherever possible. 

## Logging
log4net has been used for logging various level of informtion for the application. File level and console level logging has been implemented and log4net configuration details can be found in **App.config** file.

 ## Configuration
 As per the requirement, all the application settings are being retrieved from **App.config** file.
