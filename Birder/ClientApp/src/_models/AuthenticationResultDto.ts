
export interface AuthenticationResultDto {
    authenticationToken: string;
    failureReason: AuthenticationFailureReason;
}

export enum AuthenticationFailureReason {
    None = 0,
    EmailConfirmationRequired = 1,
    LockedOut = 2,
    Other = 3
}
