import { ObservationViewModel } from './ObservationViewModel';

export interface ObservationFeedDto {
    totalItems: number;
    // totalPages: number;
    items: ObservationViewModel[];
    displayMessage: boolean;
    message: string;
}
