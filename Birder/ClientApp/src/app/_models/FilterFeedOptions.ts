export enum ObservationFeedFilter {
    Network = 0,
    Public,
    Own
}

export namespace ObservationFeedFilter {

    export function values() {
        return Object.keys(ObservationFeedFilter).filter(
            (type) => isNaN(<any>type) && type !== 'values'
        );
    }
}
