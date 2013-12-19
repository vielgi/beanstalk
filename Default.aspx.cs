using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using Amazon.ElasticBeanstalk;

public partial class _Default : System.Web.UI.Page
{
    #region classes

    private class Item
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    #endregion

    #region properties

    private List<Amazon.ElasticBeanstalk.Model.ConfigurationOptionDescription> Options
    {
        get
        {
            return (List<Amazon.ElasticBeanstalk.Model.ConfigurationOptionDescription>)(Session["opt"] ?? null);
        }
        set
        {
            Session["opt"] = value;
        }
    }

    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DescribeConfigurationOptions();
        }
    }

    #region private

    private void DescribeConfigurationOptions()
    {
        using (var client = Amazon.AWSClientFactory.CreateAmazonElasticBeanstalkClient(GetConf()))
        {
            Amazon.ElasticBeanstalk.Model.DescribeEnvironmentsRequest req = new Amazon.ElasticBeanstalk.Model.DescribeEnvironmentsRequest();
            req.ApplicationName = "vielgi"; 

            var response = client.DescribeEnvironments(req);

            string envName = response.Environments[0].EnvironmentName;
            string envId = response.Environments[0].EnvironmentId;
            
            string health = response.Environments[0].Health.Value;
            lblHealth.Text = Server.HtmlEncode(health);
            
            string status = response.Environments[0].Status.Value;
            lblStatus.Text = Server.HtmlEncode(status);

            if (Options == null)
            {
                Amazon.ElasticBeanstalk.Model.DescribeConfigurationOptionsRequest optReq = new Amazon.ElasticBeanstalk.Model.DescribeConfigurationOptionsRequest();
                optReq.EnvironmentName = envName;
                optReq.ApplicationName = req.ApplicationName;

                var optResp = client.DescribeConfigurationOptions(optReq);
                Options = optResp.Options;
            }

            Amazon.ElasticBeanstalk.Model.DescribeConfigurationSettingsRequest setReq = new Amazon.ElasticBeanstalk.Model.DescribeConfigurationSettingsRequest();
            setReq.ApplicationName = req.ApplicationName;
            setReq.EnvironmentName = envName;

            var setResp = client.DescribeConfigurationSettings(setReq);

            gvOptions.DataSource = setResp.ConfigurationSettings[0].OptionSettings.Where(x => x.OptionName == "MinSize" || x.OptionName == "MaxSize");
            gvOptions.DataBind();

            #region buttons

            if (status.ToLower() != "ready")
            {
                btnSet1.Visible = btnSet2.Visible = false;
            }
            else
            {
                int currentMinSize = Convert.ToInt32(setResp.ConfigurationSettings[0].OptionSettings.Where(x => x.OptionName == "MinSize").Single().Value);

                btnSet2.Visible = currentMinSize == 1;
                btnSet1.Visible = !btnSet2.Visible;
            }

            #endregion

            lblGenerated.Text = string.Format("Generated on {0: H:mm:ss}", DateTime.Now);
        }
    }

    private AmazonElasticBeanstalkConfig GetConf()
    {
        AmazonElasticBeanstalkConfig conf = new AmazonElasticBeanstalkConfig();
        conf.ServiceURL = "https://elasticbeanstalk.us-west-2.amazonaws.com";

        return conf;
    }

    private void ChangeConfiguration(string minSize)
    {
        using (var client = Amazon.AWSClientFactory.CreateAmazonElasticBeanstalkClient(GetConf()))
        {
            Amazon.ElasticBeanstalk.Model.UpdateEnvironmentRequest req = new Amazon.ElasticBeanstalk.Model.UpdateEnvironmentRequest();
            req.EnvironmentName = "vielgi2-env";
            req.OptionSettings.Add(new Amazon.ElasticBeanstalk.Model.ConfigurationOptionSetting { Namespace = Options.Where(x => x.Name == "MinSize").Single().Namespace, OptionName = "MinSize", Value = minSize });
            
            var response = client.UpdateEnvironment(req);            
        }
    }

    #endregion

    #region events
    
    protected void btnUpdate_Click(object sender, EventArgs e)
    {      
        ChangeConfiguration((sender as Button).CommandArgument);

        DescribeConfigurationOptions();
    }

    protected void ctrTimer_Tick(object sender, EventArgs e)
    {
        DescribeConfigurationOptions();
    }
    #endregion
}