using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Xunit;

namespace Albelli.Jira.Tests.Search
{
    public class JiraIssuesSeed
    {
	    public static Lazy<IList<JiraIssueRecord>> Records = new Lazy<IList<JiraIssueRecord>>(() =>
	    {
		    var ordersFile = Path.Combine(Directory.GetCurrentDirectory(), "jira-short-issues.json");

		    Assert.True(File.Exists(ordersFile), "Missing issues seed file.");

		    var content = File.ReadAllText(ordersFile);

		    var records = JsonConvert.DeserializeObject<IList<JiraIssueRecord>>(content);

		    return records;
	    }, isThreadSafe: true);
	}
}
