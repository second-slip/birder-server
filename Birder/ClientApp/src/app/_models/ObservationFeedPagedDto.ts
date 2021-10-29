import { ObservationFeedDto } from "./ObservationFeedDto";

export interface ObservationFeedPagedDto {
    totalItems: number;
    items: ObservationFeedDto[];
    returnFilter: string;// ObservationFeedFilter;
}