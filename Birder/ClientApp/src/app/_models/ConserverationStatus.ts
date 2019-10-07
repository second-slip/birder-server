import { BirdDetailViewModel } from './BirdDetailViewModel';

export interface ConservationStatus {
    conservationStatusId: number;
    conservationList: string;
    conservationListColourCode: string;
    description: string;
    creationDate: Date | string;
    lastUpdateDate: Date | string;
    birds: BirdDetailViewModel[];
}
