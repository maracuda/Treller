var AvatarItem = React.createClass({
    render(){
        return <div className="user-avatar-block" title={this.props.UserFullName} style={{ backgroundImage: this.props.AvatarSrc ? "url('{0}')".format(this.props.AvatarSrc) : "none" }}>
            {!this.props.AvatarSrc && this.props.UserFullName
                && <span className="user-avatar-initial">{this.props.UserFullName.charAt(0)}</span>
            }
        </div>
    }
});