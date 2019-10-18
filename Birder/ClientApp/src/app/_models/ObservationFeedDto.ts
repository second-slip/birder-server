import { ObservationViewModel } from './ObservationViewModel';

export interface ObservationFeedDto {
    totalItems: number;
    items: ObservationViewModel[];
}
