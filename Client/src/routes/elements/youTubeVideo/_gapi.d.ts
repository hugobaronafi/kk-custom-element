export namespace gapi {
  namespace client {
    function setApiKey(apiKey: string): void;

    namespace youtube {
      namespace search {
        function list(options: {
          part: string[];
          maxResults: number;
          type: string;
          pageToken: string;
          q: string;
        }): Promise<{ result: IYouTubeSearchListResponse }>;
      }
    }
  }
}

export interface IYouTubeSearchListResponse {
  kind: string;
  etag: string;
  nextPageToken: string;
  prevPageToken: string;
  regionCode: string;
  pageInfo: {
    totalResults: number;
    resultsPerPage: number;
  };
  items: IYouTubeSearchResult[];
}

export interface IYouTubeSearchResult {
  kind: string;
  etag: string;
  id: {
    kind: string;
    videoId: string;
  };
  snippet: IYouTubeSearchResultSnippet;
}

export interface IYouTubeSearchResultSnippet {
  publishedAt: Date;
  channelId: string;
  title: string;
  description: string;
  thumbnails: {
    default: {
      url: string;
      width: number;
      height: number;
    };
    medium: {
      url: string;
      width: number;
      height: number;
    };
    high: {
      url: string;
      width: number;
      height: number;
    };
  };
  channelTitle: string;
  liveBroadcastContent: string;
}
