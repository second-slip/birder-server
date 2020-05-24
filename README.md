# Birder
Birder is a social network-style platform for logging, sharing and analysing bird observations. Ultimately, the aim is to feed an algorithm which will help with bird identification. 


### Technical information

Birder is an Angular 9 single-page application and an ASP.NET Core 3.1 API.

The Angular web app 'environments' folder is not tracked.  You will need add an environments folder and at least the environment.ts file to your cloned copy.  Here's the file structure:

```
└──Birder/src/environments/
   └──environment.ts
   └──environment.prod.ts
```

The environment.ts file should look like this:

```
export const environment = {
    production: false,
    corsAnywhereUrl: 'https://cors-anywhere.herokuapp.com/',
    xenoCantoApiBaseUrl: 'https://www.xeno-canto.org/api/2/recordings?query=',
    flickrApiKey: '<<<add Flickr Api Key>>>',
    flickrApiUrl: 'https://api.flickr.com/services/rest/',
    cookieDomain: 'localhost',
    mapKey: '<<<add Google Maps Api Key>>>' 
  };
  ```

#### Sample data

T-SQL scripts for adding sample data to the database can be provided on request
