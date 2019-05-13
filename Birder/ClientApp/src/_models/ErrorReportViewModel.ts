export class ErrorReportViewModel {
    constructor() { this.modelStateErrors = []; }

    type: string;
    errorNumber: number;
    message: string;
    serverCustomMessage: string;
    friendlyMessage: string;
    modelStateErrors: string[];
}
