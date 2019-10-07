import { AuthenticationFailureReason } from './AuthenticationResultDto';

export class ErrorReportViewModel {
    constructor() { this.modelStateErrors = []; }

    type: string;
    errorNumber: number;
    message: string;
    serverCustomMessage: string;
    friendlyMessage: string;
    modelStateErrors: string[];
}

export class ServerModelStateErrorViewModel extends ErrorReportViewModel {
    
}

export class AuthenticationErrorViewModel extends ErrorReportViewModel {
    failureReason: AuthenticationFailureReason;
}

