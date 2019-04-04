import { BirdSummaryViewModel } from './BirdSummaryViewModel';

export interface TweetDay {
    TweetDayId: number;
    DisplayDay: Date | string;
    CreationDate: Date | string;
    LastUpdateDate: Date | string;
    Bird: BirdSummaryViewModel;
}
