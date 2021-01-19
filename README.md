# Birder
Birder is a social network-style platform for logging, sharing, and analysing bird observations. Ultimately, the aim is to feed the data into an algorithm which will help with bird identification. 


### Technical information

Birder is composed of an Angular 11 single-page application served by an ASP.NET Core 5.0 API.

The Angular web app 'environments' folder is not tracked.  You will need add an environments folder and at least the environment.ts file to your cloned copy.  Here's the file structure:

```
└──Birder/src/environments/
   └──environment.ts
   └──environment.prod.ts
```

The (necessary) environment.ts file should look like this:

```
export const environment = {
    production: false,
    cookieDomain: 'localhost',
    mapKey: '<<<add Google Maps Api Key>>>' 
  };
  ```

#### Sample data

T-SQL scripts for adding sample data to the database can be provided on request
