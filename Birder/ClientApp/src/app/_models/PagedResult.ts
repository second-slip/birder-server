export interface PagedResultBase {
    currentPage: number;
    pageCount: number;
    pageSize: number;
    rowCount: number;
    firstRowOnPage: number;
    lastRowOnPage: number;
}

export interface PagedResult<T> extends PagedResultBase { //where T : class
    results: T[];
}
